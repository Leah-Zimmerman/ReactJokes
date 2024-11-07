import React, { useState } from "react";
import axios from "axios";
import { useNavigate } from "react-router-dom";

const Signup = () => {

    const [formData, setFormData] = useState({ firstName: '', lastName: '', email: '', password: '' });
    const nav = useNavigate();

    const onTextChange = (e) => {
        const copy = { ...formData };
        copy[e.target.name] = e.target.value;
        setFormData(copy);
    }
    const onFormSubmit = async(e) => {
        e.preventDefault();
        await axios.post('/api/account/signup',formData);
        nav('/login');
    }

    return (<>
        <div className="row" style={{ minHeight: '80vh', alignItems: 'center', display: 'flex' }}>
            <div className="col-md-6 offset-md-3 bg-light p-4 shadow">
                <h4>Sign up for a new account</h4>
                <form onSubmit={onFormSubmit}>
                    <input type="text" className="form-control" placeholder="First Name" name="firstName" value={formData.firstName} onChange={onTextChange}></input>
                    <br/>
                    <input type="text" className="form-control" placeholder="Last Name" name="lastName" value={formData.lastName} onChange={onTextChange}></input>
                    <br/>
                    <input type="text" className="form-control" placeholder="Email" name="email" value={formData.email} onChange={onTextChange}></input>
                    <br/>
                    <input type="password" className="form-control" placeholder="Password" name="password" value={formData.password} onChange={onTextChange}></input>
                    <br/>
                    <button className="btn btn-primary">Signup</button>
                </form>
            </div>
        </div>
    </>)
}
export default Signup;