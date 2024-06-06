import { useEffect, useState } from 'react';
import DateFormatter from '../helpers/dateHelper';

function Home() {

    document.title = "Welcome";
    const [userInfo, setUserInfo] = useState({});

    useEffect(() => {
        const user = localStorage.getItem("user");
        fetch("weatherforecast/home/" + user, {
            method: "GET",
            credentials: "include"
        }).then(response => response.json()).then(data => {
            setUserInfo(data.userInfo);
            console.log("user info: ", data.userInfo);
        }).catch(error => {
            console.log("Error home page: ", error);
        });
    }, []);

    return (
        <section className='page'>
            <header>
                <h1>Welcome to your page</h1>
            </header>
            {
                userInfo ?
                    <div>
                        <table>
                            <thead>
                                <tr>
                                    <th>Name</th>
                                    <th>Email</th>
                                    <th>Last Login Date</th>
                                    <th>Login Count</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <td>{userInfo.name}</td>
                                    <td>{userInfo.email}</td>
                                    <td>{DateFormatter(userInfo.lastLoginDate, "DD/MM/yyyy HH:mm:ss")}</td>
                                    <td>{userInfo.loginCount}</td>
                                </tr>
                            </tbody>
                        </table>
                    </div> :
                    <div className='warning'>
                        <div>Access Denied!!!</div>
                    </div>
            }
        </section>
    );
}

export default Home;