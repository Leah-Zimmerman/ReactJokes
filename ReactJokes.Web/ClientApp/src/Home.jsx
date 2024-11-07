import React, { useEffect, useState, useRef } from "react";
import axios from "axios";
import { useAuth } from "./AuthContext";
import { Link } from "react-router-dom";

const Home = () => {

    const [joke, setJoke] = useState({});
    const [count, setCount] = useState({
        likes: 0,
        dislikes: 0
    });
    const [userInteraction, setUserInteraction] = useState(null);
    const [time,setTime] =useState(null);

    const { user } = useAuth();
    const jokeRef = useRef(joke);

    useEffect(() => {
        jokeRef.current = joke;
    }, [joke])

    useEffect(() => {
        const getJokeAndLikes = async () => {
            const { data: jokeData } = await axios.get('/api/joke/getjoke');
            setJoke(jokeData);

            if (jokeData.originId) {
                const { data: likesData } = await axios.get('/api/joke/getLikesForJoke', { params: { id: jokeData.originId } });
                setCount({
                    likes: likesData.likesCount,
                    dislikes: likesData.dislikesCount
                });
                if (!!user) {
                    const { data: interactionData } = await axios.get('/api/joke/getUserInteraction', { params: { originId: jokeData.originId } });
                    setUserInteraction(interactionData);
                    if (interactionData) {
                        const { data: timeData } = await axios.get('/api/joke/getTime', { params: { originId: jokeData.originId } });
                        const dateTime = new Date(timeData);
                        setTime(dateTime);
                    }
                }
            }

        };
        getJokeAndLikes();

        const intervalId = setInterval(async () => {
            const currentJoke = jokeRef.current;
            if (currentJoke && currentJoke.originId) {
                const { data } = await axios.get('/api/joke/getLikesForJoke', { params: { id: currentJoke.originId } })
                setCount({
                    likes: data.likesCount,
                    dislikes: data.dislikesCount
                });
            }

        }, 500);
        return () => clearInterval(intervalId);
    }, []);

    const addUserLike = async (isLiked) => {
        await axios.post('/api/joke/adduserjoke', { jokeId: joke.originId, liked: isLiked });
        setUserInteraction(isLiked ? 'liked' : 'disliked');
    }

    return (<>
        <div className="row" style={{ minHeight: '80vh', display: 'flex', alignItems: 'center' }}>
            <div className="col-md-6 offset-md-3 bg-light p-4 shadow">
                <h4>{joke.setup}</h4>
                <h4>{joke.punchline}</h4>
                {!!user && (
                    <>
                        <button className="btn btn-primary" onClick={() => addUserLike(true)} disabled={userInteraction === 'liked' || (time &&(new Date().getTime() - time.getTime()) > 24 * 60 * 60 * 1000)} >Like</button>
                        <button className="btn btn-danger" onClick={() => addUserLike(false)} disabled={userInteraction === 'disliked'|| (time && (new Date().getTime() - time.getTime()) > 24 * 60 * 60 * 1000)}>Dislike</button>
                    </>)}
                {!user && <Link to='/login'>Log in to your account to like / dislike this joke</Link>}
                <br />
                <h4>Likes: {count.likes}</h4>
                <h4>Dislikes: {count.dislikes}</h4>
                <button className="btn btn-link" onClick={() => window.location.reload()}>Refresh</button>
            </div>
        </div>
    </>)
}

export default Home;