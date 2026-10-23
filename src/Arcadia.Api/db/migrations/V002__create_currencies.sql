CREATE TABLE currencies(
    code            text PRIMARY KEY,
    display_name    text NOT NULL,
    is_premium      bool NOT NULL,    
    
    CONSTRAINT currencies_code_ck CHECK(code ~ '^[A-Z]{1,20}$')
);

INSERT INTO currencies(code, display_name, is_premium)
VALUES('COIN', 'Coins', false),
        ('GEM', 'Gems', true);