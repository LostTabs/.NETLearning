CREATE TABLE inventory(
    player_id       bigint NOT NULL,
    item_id         bigint NOT NULL,
    quantity        bigint NOT NULL,
    acquired_at     timestamptz NOT NULL DEFAULT now(),
    
    CONSTRAINT inventory_pk PRIMARY KEY (player_id, item_id),
    -- If a player is deleted, it doesn't make sense to keep their inventory, so delete the inventory with it
    CONSTRAINT inventory_player_id_fk FOREIGN KEY (player_id) REFERENCES players (id) ON DELETE CASCADE,
    -- If an item is deleted, players would lose items. It makes more sense to migrate/delete all the items from
    -- players inventories before allowing the deletion of an item.
    CONSTRAINT inventory_item_id_fk FOREIGN KEY (item_id) REFERENCES items (id) ON DELETE RESTRICT,
    -- We can't enforce the stackable rule in the database because tables cant look at other tables checks.
    -- This should be enforced at the application logic. There is also the option of using a trigger, but
    -- considering the system being built, I think checking at the application level if the player already has
    -- an item would be better.
    CONSTRAINT inventory_quantity_ck CHECK ( quantity > 0 )
);

CREATE INDEX inventory_item_id_ix ON inventory (item_id);