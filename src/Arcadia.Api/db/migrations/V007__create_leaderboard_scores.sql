CREATE TABLE leaderboard(
    player_id bigint PRIMARY KEY,
    score bigint NOT NULL,
    updated_at timestamptz NOT NULL DEFAULT now(),
    
    --If a player is deleted, it doesn't make sense to keep them in the leaderboards, or worst, there is an
    --entry in the leaderboard for an unknown player. Better to remove it.
    CONSTRAINT leaderboard_player_id_fk FOREIGN KEY (player_id) REFERENCES players (id) ON DELETE CASCADE,
    CONSTRAINT leaderboard_scores_score_ck CHECK (score >= 0)
);