CREATE TABLE player_balances(
    player_id       bigint NOT NULL,
    currency_code   text NOT NULL, 
    balance         bigint NOT NULL DEFAULT 0,
    updated_at      timestamptz NOT NULL DEFAULT now(), --This should be maintained by API code.
    
    CONSTRAINT player_balances_pk PRIMARY KEY (player_id, currency_code),
    -- A balance with no player makes no sense. If we are deleting a player, the balance should follow
    -- instead of leaving orphans or blocking the delete process. 
    CONSTRAINT player_balances_player_fk FOREIGN KEY (player_id) REFERENCES players (id) ON DELETE CASCADE,
    -- Deleting a currency that a player has a balance on would silently delete their currency. This action
    -- should be refused and the balance should be migrated or zeroed before removing the currency
    CONSTRAINT player_balances_currency_fk FOREIGN KEY (currency_code) REFERENCES currencies (code) ON DELETE RESTRICT,
    CONSTRAINT player_balances_balance_ck CHECK ( balance >= 0 )
);

