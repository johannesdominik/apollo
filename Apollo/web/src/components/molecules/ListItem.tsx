import React from 'react';
import { FaEdit, FaTrashAlt } from 'react-icons/fa';
import Button from '../atoms/Button';
import './ListItem.css';
import {Show} from '../../services/showService';

interface Props {
  editAction?: (item: any) => void;
  deleteAction: (item: any) => void;
  children: string;
  extraInfo: string;
  show?: Show;
}

function ListItem({ editAction, deleteAction, children, extraInfo, show }: Props) {
  const deleteItem = () => {
    if (show) {
      deleteAction(show);
    } else {
      deleteAction(children);
    }
  };

  return (
    <div className="list-item">
      <div className="list-item__text-container">
        <p className="list-item__text list-item__text--bold">{ children }</p>
        <p className="list-item__text">{ extraInfo }</p>
      </div>
      <div className="list-item__button-container">
        {editAction &&
          <Button onClick={() => editAction(children)} buttonType="transparent">
            <FaEdit className="list-item__icon--white"/>
          </Button>
        }
        <Button onClick={deleteItem} buttonType="transparent">
          <FaTrashAlt className="list-item__icon--red"/>
        </Button>
      </div>
    </div>
  );
}

export default ListItem;