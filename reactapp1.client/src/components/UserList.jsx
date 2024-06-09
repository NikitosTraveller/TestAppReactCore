import { useEffect, useState, useRef } from 'react';
import formatDate from '../helpers/dateHelper';
import { adminRole, isAdmin, isRegular, isSuperAdmin, regularRole } from '../helpers/userHelper';
import Avatar from 'react-avatar';
import axios from 'axios';

function UserList() {

    document.title = "Users";
    const [users, setUsers] = useState([]);

    const [image, setImage] = useState("");
    const inputFile = useRef();

    const currentUserRole = localStorage.getItem("role");
    const currentUserEmail = localStorage.getItem("user");

    const [selectedUserId, setSelectedUserId] = useState("");

    const showChangeRoleButton = (user) =>
    {
        if (isSuperAdmin(currentUserRole)) {
            return user.email !== currentUserEmail;
        }

        if (isRegular(currentUserRole)) {
            return false;
        }

        if (isAdmin(currentUserRole)) {
            return !isSuperAdmin(user.roleName) && (user.email !== currentUserEmail);
        }
        return false;
    };

    const showDeleteButton = (user) =>
    {
        if (isSuperAdmin(currentUserRole)) {
            return user.email !== currentUserEmail;
        }

        if (isRegular(currentUserRole)) {
            return user.email === currentUserEmail;
        }

        if (isAdmin(currentUserRole)) {
            return isRegular(user.roleName);
        }
        return false;
    };

    const showSetAvatarButton = (user) =>
    {
        if (isSuperAdmin(currentUserRole)) {
            return true;
        }

        if (isRegular(currentUserRole)) {
            return user.email === currentUserEmail;
        }

        if (isAdmin(currentUserRole)) {
            return !isSuperAdmin(user.roleName);
        }
        return false;
    };

    async function handleFileUpload(e) {
        const file = e.target.files[0];
        const fileName = file.name;
        if (file) {
            const response = await axios.post('weatherforecast/avatar',
                {
                    formFile: file,
                    fileName: fileName,
                    userId: selectedUserId,
                },
                {
                    headers: {
                        'Content-Type': 'multipart/form-data'
                    }
                }).catch(error => {
                    console.log("Error upload: ", error);
                });

            //setImage(files[0]);
        }
    };

    const selectFileHandler = (userId) => {
        setSelectedUserId(userId);
        inputFile.current.click();
    }

    useEffect(() => {

        fetch("weatherforecast/users", {
            method: "GET",
            credentials: "include"
        }).then(response => response.json()).then(data => {
            setUsers(data.users);
            console.log("users: ", data.users);
        }).catch(error => {
            console.log("Error users page: ", error);
        });
    }, []);

    async function handleDelete(id) {
        let result = await axios.delete("weatherforecast/delete/" + id)
            .then(response => response.json()).then(data =>
            {
                const updatedList = users.filter(user => user.id !== id);
                setUsers(updatedList);
        }).catch(error => {
            console.log("Error home page: ", error);
        });
    }

    async function handleRoleChange(user) {
        let result = await axios.put("weatherforecast/changerole", {
            role: isAdmin(user.roleName) ? regularRole : adminRole,
            id: user.id,
        }).then(function (response) {
            const _updatedUser = response.data.updatedUser;

            const currentUserIndex = users.findIndex((u) => u.email === _updatedUser.email);
            const updatedUser = { ...users[currentUserIndex], roleName: _updatedUser.roleName };

            const newList = [
                ...users.slice(0, currentUserIndex),
                updatedUser,
                ...users.slice(currentUserIndex + 1)
            ];
            setUsers(newList);;
        }).catch(error => {
            console.log("Error home page: ", error);
        });
    }

    return (
        <section className='admin-page page'>
            <header>
                <h1>Users</h1>
            </header>
                {
                users &&
                <>
                    <input
                        style={{ display: "none" }}
                        ref={inputFile}
                        onChange={handleFileUpload}
                        type="file"
                    />
                            <table>
                                    <thead>
                                        <tr>
                                            <th></th>
                                            <th>UserName</th>
                                            <th>Email</th>
                                            <th>Last Login Date</th>
                                            <th>Login Count</th>
                                            <th>Role Name</th>
                                            <th>Change Role</th>
                                            <th>Set Avatar</th>
                                            <th>Delete</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        {
                                            users.map((user) =>
                                                <tr key={user.id}>
                                                    <td><Avatar src={ user.avatar} size="50" round={true} /></td>
                                                    <td>{user.userName}</td>
                                                    <td>{user.email}</td>
                                                    <td>{formatDate(user.lastLoginDate, "DD/MM/yyyy HH:mm:ss")}</td>
                                                    <td>{user.loginCount}</td>
                                                    <td>{user.roleName}</td>
                                                    <td>
                                                        {
                                                            showChangeRoleButton(user) &&
                                                            <button onClick={handleRoleChange.bind(null, user)}>Make {isRegular(user.roleName) ? adminRole : regularRole}</button>
                                                        }      
                                                    </td>
                                                    <td>
                                                        {
                                                            showSetAvatarButton(user) &&
                                                            <button onClick={selectFileHandler.bind(null, user.id)}>Choose File</button>
                                                        }
                                                    </td>
                                                    <td>
                                                        {
                                                            showDeleteButton(user) &&
                                                            <button onClick={handleDelete.bind(null, user.id)}>Delete</button>
                                                        }
                                                    </td>
                                                </tr>
                                            )
                                        }
                                    </tbody>
                                </table>
                            </>
                }
        </section>
    );
}

export default UserList;