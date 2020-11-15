﻿using Apollo.Core.Interface.Daos;
using Apollo.Domain;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;

namespace Apollo.Core.Daos
{
    public abstract class ShowDao : IShowDao
    {
        static string BaseDirectory = Directory.GetCurrentDirectory() + "\\..\\..\\..\\";
        private readonly AdoTemplate template;

        public ShowDao(IConnectionFactory connectionFactory)
        {
            template = new AdoTemplate(connectionFactory);
        }

        public virtual async Task<bool> DeleteAsync(Show show)
        {
            return (await template.ExecuteAsync(
                "DELETE FROM Movie WHERE StartsAt=@sa AND shows=@sh AND playsIn=@pi",
                new QueryParameter("@sa", show.StartsAt),
                new QueryParameter("@sh", show.Movie.Title),
                new QueryParameter("@pi", show.CinemaHall.Name)
                )) == 1;
        }

        public virtual async Task<IEnumerable<Show>> FindAllAsync()
        {
            return await template.QueryAsync<Show>("SELECT * FROM Show", MapRowToShow);
        }

        public virtual async Task<IEnumerable<Show>> FindByCinemaHall(CinemaHall cinemaHall)
        {
            return await template.QueryAsync<Show>(
                "SELECT * FROM Show WHERE playsIn=@ch",
                MapRowToShow,
                new QueryParameter("@ch", cinemaHall.Name));
        }

        public virtual async Task<IEnumerable<Show>> FindByDateAsync(DateTime date)
        {
            return await template.QueryAsync<Show>(
                "SELECT * FROM Show WHERE StartsAt>@date",
                MapRowToShow,
                new QueryParameter("@date", date));
        }

        public virtual async Task<IEnumerable<Show>> FindByExactDateAsync(DateTime date)
        {
            return await template.QueryAsync<Show>(
                "SELECT * FROM Show WHERE StartsAt>=@ds AND StartsAt<@de",
                MapRowToShow,
                new QueryParameter("@ds", new DateTime(date.Year, date.Month, date.Day, 0, 0, 0)),
                new QueryParameter("@de", new DateTime(date.Year, date.Month, date.Day, 23, 59, 59)));
        }

        public virtual async Task<IEnumerable<Show>> FindByMovie(Movie movie)
        {
            return await template.QueryAsync<Show>(
                "SELECT * FROM Show WHERE shows=@title",
                MapRowToShow,
                new QueryParameter("@title", movie.Title));
        }

        public virtual async Task<bool> InsertAsync(Show show)
        {
            return (await template.ExecuteAsync(
                "INSERT INTO Show (StartsAt, shows, playsIn) VALUES (@sa, @shows, @pi)",
                new QueryParameter("@sa", show.StartsAt),
                new QueryParameter("@shows", show.Movie.Title),
                new QueryParameter("@pi", show.CinemaHall.Name)
                )) == 1;
        }

        private async Task<Show> MapRowToShow(IDataRecord row)
        {
            IConfiguration config = ConfigurationUtil.GetConfiguration(BaseDirectory);
            IConnectionFactory connectionFactory = DefaultConnectionFactory.FromConfiguration(config, "ApolloTestDbConnection");

            IMovieDao movieDao = new MSSQLMovieDao(connectionFactory);
            ICinemaHallDao cinemaHallDao = new MSSQLCinemaHallDao(connectionFactory);

            return new Show
            {
                StartsAt = (DateTime)row["StartsAt"],
                Movie = await movieDao.FindByTitleAsync(row["shows"].ToString()),
                CinemaHall = await cinemaHallDao.FindByNameAsync(row["playsIn"].ToString())
            };
        }
    }
}
