import { useEffect, useState } from 'react';
import DateFormatter from '../helpers/dateHelper';
//import { DeleteIcon } from '@mui/icons-material';

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
                            <table>
                                <thead>
                                    <tr>
                                        <th>UserName</th>
                                        <th>Email</th>
                                        <th>Last Login Date</th>
                                        <th>Login Count</th>
                                        <th>Role Name</th>
                                        <th></th>
                                    </tr>
                                </thead>
                                <tbody>
                                    {
                                        users.map((user) =>
                                         <tr key={user.id}>
                                            <td>{user.userName}</td>
                                            <td>{user.email}</td>
                                            <td>{DateFormatter(user.lastLoginDate, "DD/MM/yyyy HH:mm:ss")}</td>
                                            <td>{user.loginCount}</td>
                                            <td>{user.isAdmin ? "Admin" : "Regular"}</td>
                                            <td><button>Delete</button></td>
                                        </tr>
                                        )
                                    }
                                </tbody>
                            </table>
                }
            </section>
        </section>
    );
}

export default UserList;