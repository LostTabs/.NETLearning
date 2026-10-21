CREATE TABLE items(
    id bigint GENERATED ALWAYS AS IDENTITY PRIMARY KEY ,
    sku text NOT NULL,
    display_name text NOT NULL,
    price_amount bigint NOT NULL,
    price_currency_code text NOT NULL,
    stackable bool NOT NULL DEFAULT true,
    
    -- SKU is the business identifier, which can be changed, renamed, reformatted. Other tables can reference
    -- the surrogate id instead so a sku change doesn't affect other tables.
    CONSTRAINT items_sku_uq UNIQUE (sku),
    CONSTRAINT item_sku_ck CHECK ( sku ~ '[A-Z0-9]{3,16}$' ),
    CONSTRAINT items_price_amount_ck CHECK ( price_amount > 0 ),
    -- By deleting the currency, we could make the item not purchasable. We should migrate all currencies
    -- before allowing for deletion
    CONSTRAINT items_price_currency_fk FOREIGN KEY (price_currency_code) REFERENCES currencies (code) ON DELETE RESTRICT 
);