

function logoutConfirm() {
    if (confirm('¿Desea cerrar sesión?')) {
        window.location.href = '/Home/Logout';
    }
}
