import { useState, useEffect } from "react";
import { Outlet, Navigate } from "react-router-dom";

function ProtectedRoutes() {

    const [isLogged, setIsLogged] = useState(false);
    const [waiting, setWaiting] = useState(true);

    useEffect(() => {
        fetch('/weatherforecast/validate/', {
            method: "GET",
            credentials: "include"
        }).then(response => {
            if (response.ok) {
                setWaiting(false);
                setIsLogged(true);
            }
            return response.json();
        }).then(data => {
            localStorage.setItem("user", data.user.email);
            localStorage.setItem("role", data.user.roleName);
            console.log(data.user);
        }).catch(err => {
            console.log("Error protected routes: ", err);
            setWaiting(false);
            localStorage.removeItem("user");
            localStorage.removeItem("role");
        });
    }, []);

    return waiting ? <div className="waiting-page">
        <div>Waiting...</div>
    </div> :
        isLogged ? <Outlet /> : <Navigate to="/login" />;
}

export default ProtectedRoutes;