import { useEffect, useState } from 'react';

function Register() {

    document.title = "Register";

    const [error, setError] = useState("");

    const [userName, setUserName] = useState("");
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [confirmPassword, setConfirmPassword] = useState("");

    const handleChange = (e) => {
        const { name, value } = e.target;

        switch (name) {
            case "userName": {
                setUserName(value);
                break;
            }
            case "email": {
                setEmail(value);
                break;
            }
            case "password": {
                setPassword(value);
                break;
            }
            case "confirmPassword": {
                setConfirmPassword(value);
                break;
            }
        }
    };

    // dont ask an already registered user to register over and over again
    useEffect(() => {
        const user = localStorage.getItem("user");
        if (user) {
            document.location = "/";
        }
    }, []);

    async function registerHandler(e) {
        e.preventDefault();

        if (password !== confirmPassword) {
            setError("Passwords do not match.");
            return;
        }

        const response = await fetch("weatherforecast/register", {
            method: "POST",
            credentials: "include",
            body: JSON.stringify({
                email: email,
                userName: userName,
                password: password
            }),
            headers: {
                "content-type": "Application/json",
                "Accept": "application/json"
            }
        });

        const data = await response.json();

        if (response.ok) {
            document.location = "/login";
        }
        else {
            setError(data.message ?? "Something went wrong, please try again")
        }
    }

    return (
        <section className='register-page-wrapper page'>
            <div className='register-page'>
                <header>
                    <h1>Register Page</h1>
                </header>
                {error && <p className="message">{error}</p>}
                <div className='form-holder'>
                    <form action="#" className='register' onSubmit={registerHandler}>
                        <label htmlFor="userName">Username</label>
                        <br />
                        <input
                            type="text"
                            name='userName'
                            id='userName'
                            value={userName}
                            onChange={handleChange}
                            required
                        />
                        <br />
                        <label htmlFor="email">Email</label>
                        <br />
                        <input
                            type="email"
                            name='email'
                            id='email'
                            value={email}
                            onChange={handleChange}
                            required
                        />
                        <br />
                        <label htmlFor="password">Password</label>
                        <br />
                        <input
                            type="password"
                            name='password'
                            id='password'
                            value={password}
                            onChange={handleChange}
                            required
                        />
                        <br />
                        <label htmlFor="confirmPassword">Confirm Password</label>
                        <br />
                        <input
                            type="password"
                            name='confirmPassword'
                            id='confirmPassword'
                            value={confirmPassword}
                            onChange={handleChange}
                            required
                        />
                        <br />
                        <input
                            type="submit"
                            value="Register"
                            className='register btn'
                        />
                    </form>
                </div>
                <div className='my-5'>
                    <a href="/login">Login</a>
                </div>
            </div>
        </section>
    );
}

export default Register;