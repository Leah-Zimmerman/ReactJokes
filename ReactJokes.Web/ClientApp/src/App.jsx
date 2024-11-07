import React from 'react';
import { Link, Route, Routes } from 'react-router-dom';
import Layout from './Layout';
import Signup from './Signup';
import Home from './Home';
import Login from './Login';
import { AuthContextComponent } from './AuthContext';
import ViewAll from './ViewAll';

const App = () => {
    return (<>
        <AuthContextComponent>
            <Layout>
                <Routes>
                    <Route exact path="/" element={<Home />} />
                    <Route exact path="/viewall" element={<ViewAll />} />
                    <Route exact path="/signup" element={<Signup />} />
                    <Route exact path="/login" element={<Login />} />
                </Routes>
            </Layout>
        </AuthContextComponent>
    </>)
}

export default App;