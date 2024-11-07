import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import { Link } from "react-router-dom";
import { useAuth } from "./AuthContext";
import axios from "axios";

const Login = () => {

    const [loginCredentials, setLoginCredentials] = useState({ email: '', password: '' });
    const nav = useNavigate();
    const {setUser} = useAuth();

    const onTextChange = e => {
        const copy = { ...loginCredentials };
        copy[e.target.name] = e.target.value;
        setLoginCredentials(copy);
    }

    const onFormSubmit = async (e) => {
        e.preventDefault();
        var { data } = await axios.post('/api/account/login', loginCredentials);
        const isValid = !!data;
        if (isValid){
            setUser(data);
        }
        nav('/');
    }

    return (<>
        <div className="row" style={{ minHeight: '80vh', display: 'flex', alignItems: 'center' }}>
            <div className="col-md-6 offset-md-3 bg-light p-4 shadow">
                <form onSubmit={onFormSubmit}>
                    <h4>Log in to your account</h4>
                    <input className="form-control" type="text" placeholder="Email" name='email' onChange={onTextChange} value={loginCredentials.email}></input>
                    <br />
                    <input className="form-control" type="password" placeholder="Password" name='password' onChange={onTextChange} value={loginCredentials.password}></input>
                    <br />
                    <button className="btn btn-primary">Login</button>
                    <br />
                    <Link to="/signup">Sign up for a new account</Link>
                </form>
            </div>
        </div>
    </>)
}

export default Login;