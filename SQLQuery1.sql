
BEGIN TRANSACTION;
CREATE TABLE register(
id_user int identity(1,1) not null,
login_user varchar(50) not null,
password_user varchar(50) not null,
);
COMMIT TRANSACTION;

select * from register;

begin transaction;
insert into register(login_user, password_user) values ('admin', 'admin');
commit transaction;



USE TestDB