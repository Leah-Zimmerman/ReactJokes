import React, { useEffect, useState } from "react";
import axios from 'axios';

const ViewAll = () => {
    const [jokesWithLikes, setJokesWithLikes] = useState([]);

    useEffect(() => {
        const getJokesWithLikes = async () => {
            const { data } = await axios.get('/api/joke/getJokesWithLikes');
            setJokesWithLikes(data);
        }
        getJokesWithLikes();
    }, [])

    return (<>
        {jokesWithLikes.map((jwl, i) => (
            <div className="row" key={i}>
                <div className="col-md-6 offset-md-3">
                    <div className="card card-body bg-light mb-3">
                        <h5>{jwl.joke.setup}</h5>
                        <h5>{jwl.joke.punchline}</h5>
                        <span>Likes: {jwl.likesCount}</span>
                        <span>Dislikes: {jwl.dislikesCount}</span>
                    </div>
                </div>
            </div>
        ))}
    </>)
}
export default ViewAll;