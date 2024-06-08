export const isSuperAdmin = (user) => user && user.roleName && user.roleName === 'SuperAdmin';

export const isAdmin = (user) => user && user.roleName && user.roleName === 'Admin';

export const isRegular = (user) => user && user.roleName && user.roleName === 'Regular';