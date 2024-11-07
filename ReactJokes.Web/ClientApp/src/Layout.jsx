import { Link, useNavigate } from "react-router-dom";
import { Button } from "react-bootstrap";
import { useAuth } from "./AuthContext";
import axios from "axios";

function Layout({ children }) {
    const { user, setUser } = useAuth();
    const nav = useNavigate();

    const logout = async () => {
        await axios.post('/api/account/logout');
        setUser(null);
        nav('/login');
    }

    return <>
        <header>
            <nav className="navbar navbar-expand-sm navbar-toggleable-sm navbar-dark bg-dark border-bottom box-shadow mb-3">
                <div className="container-fluid">
                    <Link className="navbar-brand" to='/'>React Jokes</Link>
                    <button className="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target=".navbar-collapse" aria-controls="navbarSupportedContent"
                        aria-expanded="false" aria-label="Toggle navigation">
                        <span className="navbar-toggler-icon"></span>
                    </button>
                    <div className="navbar-collapse collapse d-sm-inline-flex justify-content-between">
                        <ul className="navbar-nav flex-grow-1">
                            <li className="nav-item">
                                <Link to='/viewall' className="nav-link text-light">View All</Link>
                            </li>
                            {!user && <li className="nav-item">
                                <Link to='/signup' className="nav-link text-light">Sign Up</Link>
                            </li>}
                            {!user && <li className="nav-item">
                                <Link to='/login' className="nav-link text-light">Log In</Link>
                            </li>}
                            {!!user && <li className="nav-item">
                                <span
                                    className="nav-link text-light custom-nav-link"
                                    onClick={logout}
                                    style={{ cursor: 'pointer' }}
                                >
                                    Log Out
                                </span>
                            </li>}
                        </ul>
                    </div>
                </div>
            </nav>
        </header >
        <div className="container">
            <main role="main">
                {children}
            </main>
        </div>
    </>
}

export default Layout;