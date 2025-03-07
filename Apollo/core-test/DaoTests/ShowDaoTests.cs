﻿using Apollo.Core.Daos;
using Apollo.Core.Interface.Daos;
using Apollo.Domain;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace Apollo.Core.Test
{
    public class ShowDaoTests
    {
        static string BaseDirectory = Directory.GetCurrentDirectory() + "\\..\\..\\..\\";
        static IConfiguration config = ConfigurationUtil.GetConfiguration(BaseDirectory);
        static IConnectionFactory connectionFactory = DefaultConnectionFactory.FromConfiguration(config, "ApolloTestDbConnection");
        IShowDao showDao = new MSSQLShowDao(connectionFactory);
        ICinemaHallDao cinemaHallDao = new MSSQLCinemaHallDao(connectionFactory);
        IMovieDao movieDao = new MSSQLMovieDao(connectionFactory);


        [Fact]
        public async void InsertAsyncTest()
        {
            Movie movie = new Movie("The Two Popes", "At a key turning point for the Catholic Church, Pope Benedict XVI forms a surprising friendship with the future Pope Francis. Inspired by true events.", "Thriller", 125.0, "Anthony Hopkins, Jonathan Pryce, Juan Minujin", "https://cdn.collider.com/wp-content/uploads/2019/07/the-two-popes-anthony-hopkins-jonathan-pryce-1.jpg", "https://www.youtube.com/watch?v=T5OhkFY1PQE");
            CinemaHall cinemaHall = await cinemaHallDao.FindByNameAsync("HALL 2");
            Show show = new Show(new DateTime(2020, 11, 26, 12, 33, 48), movie, cinemaHall);
            Assert.True(await showDao.InsertAsync(show));
            await showDao.DeleteAsync(show);
        }

        [Fact]
        public async void FindAllAsyncTest()
        {
            ICollection<Show> shows = (ICollection<Show>)await showDao.FindAllAsync();
            Assert.Equal(35, shows.Count);
        }

        [Fact]
        public async void FindByDateCinemaHallAndMovieAsyncTest()
        {
            CinemaHall cinemaHall = await cinemaHallDao.FindByNameAsync("HALL 5");
            Movie movie = await movieDao.FindByTitleAsync("Life of Pi");
            Show show = await showDao.FindByDateCinemaHallAndMovieAsync(new DateTime(2021, 01, 26, 14, 45, 00), cinemaHall, movie);
            Assert.Equal("Adventure, Drama", show.Movie.Genre);
        }

        [Fact]
        public async void FindByDateAsyncTest()
        {
            ICollection<Show> shows = (ICollection<Show>)await showDao.FindByDateAsync(new DateTime(2020, 11, 20, 00, 03, 18));
            Assert.Equal(35, shows.Count);
        }

        [Fact]
        public async void FindByExactDateAsyncTest()
        {
            ICollection<Show> shows = (ICollection<Show>)await showDao.FindByExactDateAsync(new DateTime(2021, 01, 27, 00, 03, 18));
            Assert.Equal(11, shows.Count);
        }

        [Fact]
        public async void FindByCinemaHallAsyncTest()
        {
            CinemaHall cinemaHall = await cinemaHallDao.FindByNameAsync("IMAX HALL 1");
            ICollection<Show> shows = (ICollection<Show>)await showDao.FindByCinemaHallAsync(cinemaHall);
            Assert.Equal(8, shows.Count);
        }

        [Fact]
        public async void FindByMovieAsyncTest()
        {
            Movie movie = await movieDao.FindByTitleAsync("Life of Pi");
            ICollection<Show> shows = (ICollection<Show>)await showDao.FindByMovieAsync(movie);
            Assert.Equal(5, shows.Count);
        }

        [Fact]
        public async void DeleteAsyncTest()
        {
            Movie movie = new Movie("The Two Popes", "At a key turning point for the Catholic Church, Pope Benedict XVI forms a surprising friendship with the future Pope Francis. Inspired by true events.", "Thriller", 125.0, "Anthony Hopkins, Jonathan Pryce, Juan Minujin", "https://cdn.collider.com/wp-content/uploads/2019/07/the-two-popes-anthony-hopkins-jonathan-pryce-1.jpg", "https://www.youtube.com/watch?v=T5OhkFY1PQE");
            CinemaHall cinemaHall = await cinemaHallDao.FindByNameAsync("HALL 2");
            Show show = new Show(new DateTime(2020, 1, 27, 10, 00, 00), movie, cinemaHall);
            await showDao.InsertAsync(show);
            Assert.True(await showDao.DeleteAsync(show));
        }
    }
}