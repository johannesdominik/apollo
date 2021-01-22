import React, { DetailedHTMLProps, InputHTMLAttributes, useState, FormEvent } from 'react';
import './Input.css';

interface Props extends DetailedHTMLProps<InputHTMLAttributes<HTMLInputElement>, HTMLInputElement>{
  type: string,
  children: string
}

function Input({ type = 'text', children }: Props) {
  const [value, setValue] = useState('');

  const handleInputChange = (event: FormEvent<HTMLInputElement>): void => {
    setValue((event.target as any).value);
  };

  return (
    <div className="input">
      <input className="input__input" type={type} value={value} onChange={handleInputChange} placeholder={children}/>
      <div className="input__placeholder">
        { children }
      </div>
    </div>
  );
}

export default Input;