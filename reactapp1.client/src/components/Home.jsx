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
            {
                userInfo ?
                    <div>
                        <header>
                            <h1>Welcome to your page, {userInfo.userName} !</h1>
                        </header>
                        <table>
                            <thead>
                                <tr>
                                    <th>Email</th>
                                    <th>Last Login Date</th>
                                    <th>Login Count</th>
                                    <th>Role Name</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <td>{userInfo.email}</td>
                                    <td>{DateFormatter(userInfo.lastLoginDate, "DD/MM/yyyy HH:mm:ss")}</td>
                                    <td>{userInfo.loginCount}</td>
                                    <td>{userInfo.roleName}</td>
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