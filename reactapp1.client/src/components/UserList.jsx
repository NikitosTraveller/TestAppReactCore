import { useEffect, useState } from 'react';

function UserList() {

    document.title = "Users";
    const [users, setUsers] = useState([]);

    useEffect(() => {

        fetch("weatherforecast/admin", {
            method: "GET",
            credentials: "include"
        }).then(response => response.json()).then(data => {
            setUsers(data.users);
            console.log("users: ", data.users);
        }).catch(error => {
            console.log("Error users page: ", error);
        });
    }, []);

    return (
        <section className='admin-page page'>
            <header>
                <h1>Users</h1>
            </header>
            <section>
                {
                    users &&
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
                                    {
                                        users.map((user) =>
                                         <tr key={user.email}>
                                            <td>{user.name}</td>
                                            <td>{user.email}</td>
                                            <td>{user.lastLoginDate ? user.lastLoginDate.split("T")[0] : ""}</td>
                                            <td>{user.loginCount}</td>
                                        </tr>
                                        )
                                    }
                                </tbody>
                            </table>
                        </div>
                }
            </section>
        </section>
    );
}

export default UserList;