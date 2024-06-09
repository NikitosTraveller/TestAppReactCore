const adminRole = 'Admin';

const superAdminRole = 'SuperAdmin';

const regularRole = 'Regular';

/////////////////////////////////////////////////////

export const isSuperAdmin = (roleName) => roleName === superAdminRole;

export const isAdmin = (roleName) => roleName === adminRole;

export const isRegular = (roleName) => roleName === regularRole;