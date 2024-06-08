import { useEffect, useState } from 'react';
import DateFormatter from '../helpers/dateHelper';
import Avatar from 'react-avatar';
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
                        <br />
                        <header>
                            <Avatar googleId="118096717852922241760" size="100" round={true} />
                            <span>Welcome to your page, {userInfo.userName} !</span>
                        </header>
                        <br/>
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