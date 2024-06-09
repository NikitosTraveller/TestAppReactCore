import { useEffect, useState, useRef } from 'react';
import formatDate from '../helpers/dateHelper';
import Avatar from 'react-avatar';
import axios from 'axios';
import { API_URL } from '../../apiUrl.js';
import { useNavigate } from "react-router-dom";
import { isSuperAdmin} from '../helpers/userHelper';

function Home() {

    document.title = "Welcome";
    const [userInfo, setUserInfo] = useState({});

    const [possibleToDelete, setPossibleToDelete] = useState(false);

    const navigate = useNavigate();

    const [avatar, setAvatar] = useState("");

    const inputFile = useRef(null);

    useEffect(() => {
        const user = localStorage.getItem("user");
        fetch("weatherforecast/home/" + user, {
            method: "GET",
            credentials: "include"
        }).then(response => response.json()).then(data => {
            setAvatar(API_URL + data.userInfo.avatar);
            setPossibleToDelete(isSuperAdmin(data.userInfo.roleName));
            setUserInfo(data.userInfo);
            console.log("user info: ", data.userInfo);
        }).catch(error => {
            console.log("Error home page: ", error);
        });
    }, []);

    async function handleFileUpload(e) {
        const file = e.target.files[0];
        const fileName = file.name;
        if (file) {
            const response = await axios.post('weatherforecast/avatar',
            {
                formFile: file,
                fileName: fileName,
                userId: userInfo.id,
            },
            {
                headers: {
                    'Content-Type': 'multipart/form-data'
                }
            }).catch(error =>
            {
                console.log("Error upload: ", error);
            });
        }
    };

    const selectFileHandler = () => {
        inputFile.current.click();
    }

    async function handleDelete() {
        let result = await axios.delete("weatherforecast/delete/" + userInfo.id)
            .then(response => {
                localStorage.removeItem("user");
                localStorage.removeItem("role");
                navigate("/login");
            }).catch(error => {
                console.log("Error delete current user: ", error);
            });
    }

    const isAvailiableToDelete = () => !isSuperAdmin(userInfo.roleName);

    return (
        <section className='page'>
            {
                userInfo ?
                    <div>
                        <br />
                        <header>
                            <Avatar onClick={selectFileHandler} src={avatar} size="100" round={true} />
                            <span>Welcome to your page, {userInfo.userName}!</span>
                        </header>
                        <br/>
                        <table>
                            <thead>
                                <tr>
                                    <th>Email</th>
                                    <th>Last Login Date</th>
                                    <th>Login Count</th>
                                    <th>Role Name</th>
                                    <th>Delete Account</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <td>{userInfo.email}</td>
                                    <td>{formatDate(userInfo.lastLoginDate, "DD/MM/yyyy HH:mm:ss")}</td>
                                    <td>{userInfo.loginCount}</td>
                                    <td>{userInfo.roleName}</td>
                                    <td>
                                        <button hidden={possibleToDelete} onClick={handleDelete}>Delete</button>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        <input
                            style={{ display: "none" }}
                            ref={inputFile}
                            onChange={handleFileUpload}
                            type="file"
                        />
                    </div> :
                    <div className='warning'>
                        <div>Access Denied!!!</div>
                    </div>
            }
        </section>
    );
}

export default Home;