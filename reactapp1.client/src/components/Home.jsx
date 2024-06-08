import { useEffect, useState, useRef } from 'react';
import DateFormatter from '../helpers/dateHelper';
import Avatar from 'react-avatar';
import axios from 'axios';
function Home() {

    document.title = "Welcome";
    const [userInfo, setUserInfo] = useState({});

    const [image, setImage] = useState("");
    const inputFile = useRef(null);

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

            //setImage(files[0]);
        }
    };

    const selectFileHandler = () => {
        inputFile.current.click();
    }

    return (
        <section className='page'>
            {
                userInfo ?
                    <div>
                        <br />
                        <header>
                            <Avatar onClick={selectFileHandler} googleId="118096717852922241760" size="100" round={true} />
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