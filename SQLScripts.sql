begin transaction;
create table lobbies(
uid_lobby varchar(20) not null primary key,
);
commit transaction;

begin transaction;
create table users(
uid_user varchar(20) not null primary key,
uid_lobby varchar(20) not null,
"user_name" varchar(20) not null,
avatar int not null,
vote VARCHAR(20),
choice int
CONSTRAINT FK_users_lobbies FOREIGN KEY (uid_lobby) REFERENCES lobbies (uid_lobby)
);
commit transaction;

begin transaction;
CREATE TABLE cards (
    uid_user varchar(20),
    card1 INT,
    card2 INT,
    card3 INT,
    card4 INT,
    card5 INT,
    card6 INT,
    CONSTRAINT FK_cards_users FOREIGN KEY (uid_user) REFERENCES users (uid_user)
);
COMMIT TRANSACTION;

CREATE TABLE IsOpen (
    uid_user varchar(20) PRIMARY KEY,
    card11 bit,
    card22 bit,
    card33 bit,
    card44 bit,
    card55 bit,
    card66 bit,
    CONSTRAINT FK_cards_is_open_users FOREIGN KEY (uid_user) REFERENCES users (uid_user)
);


select * from lobbies;

select * from users;

select * from cards;

select * from IsOpen;


delete from lobbies;

delete from users;

delete from cards;

delete from IsOpen;

