CREATE TABLE players (
    id          bigint      GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    username    text        NOT NULL,
    email       text        NOT NULL,
    country     char(2)     NOT NULL,
    created_at  timestamptz NOT NULL DEFAULT now(),
    
    CONSTRAINT players_username_uq     UNIQUE (username),
    CONSTRAINT players_email_uq        UNIQUE (email),
    CONSTRAINT players_username_len_ck CHECK (char_length(username) BETWEEN 3 AND 20),
    CONSTRAINT players_country_ck      CHECK (country ~ '^[A-Z]{2}$')
);