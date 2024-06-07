import { useEffect, useState } from 'react';

function Login() {

    document.title = "Login";

    const [error, setError] = useState("");

    const [email, setEmail] = useState("");
    const [rememberMe, setRememberMe] = useState(false);
    const [password, setPassword] = useState("");

    const handleChange = (e) => {
        const { name, value } = e.target;

        switch (name) {
            case "password": {
                setPassword(value);
                break;
            }
            case "email": {
                setEmail(value);
                break;
            }
            case "rememberMe": {
                setRememberMe(e.target.checked);
                break;
            }
        }
    };

    // dont ask an already logged in user to login over and over again
    useEffect(() => {
        const user = localStorage.getItem("user");
        if (user) {
            document.location = "/";
        }
    }, []);

    async function loginHandler(e) {
        e.preventDefault();

        const response = await fetch("weatherforecast/login", {
            method: "POST",
            credentials: "include",
            body: JSON.stringify({
                email: email,
                password: password,
                rememberMe: rememberMe
            }),
            headers: {
                "content-type": "Application/json",
                "Accept": "application/json"
            }
        });

        const data = await response.json();

        if (response.ok) {
            localStorage.setItem("user", email);
            document.location = "/";
        }
        else {
            setError(data.message ?? "Something went wrong, please try again");
            console.log("login error: ", data);
        }

    }

    return (
        <section className='login-page-wrapper page'>
            <div className='login-page'>
                <header>
                    <h1>Login Page</h1>
                </header>
                {error && <p className="message">{error}</p>}
                <div className='form-holder'>
                    <form action="#" className='login' onSubmit={loginHandler}>
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
                        <input
                            type="checkbox"
                            name='rememberMe'
                            checked={rememberMe}
                            onChange={handleChange}
                            id='rememberMe'
                        />
                        <label htmlFor="remember">Remember Me?</label>
                        <br />
                        <br />
                        <input
                            type="submit"
                            value="Login"
                            className='login btn'
                        />
                    </form>
                </div>
                <div className='my-5'>
                    <a href="/register">Register</a>
                </div>
            </div>
        </section>
    );
}

export default Login;