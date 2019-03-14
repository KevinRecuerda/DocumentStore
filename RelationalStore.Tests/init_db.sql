
CREATE SCHEMA IF NOT EXISTS rel;

DROP TABLE IF EXISTS rel.users     CASCADE;
DROP TABLE IF EXISTS rel.addresses CASCADE;

CREATE TABLE rel.addresses (
    id      BIGSERIAL PRIMARY KEY,
    country TEXT,
    city    TEXT,
    street  TEXT
);

CREATE TABLE rel.users (
    id          BIGSERIAL PRIMARY KEY,
    first_name  TEXT,
    last_name   TEXT,
    internal    TEXT,
    user_name   TEXT,
    gender      INT,
    address_id  BIGINT NOT NULL,
    CONSTRAINT fk__users__addresses FOREIGN KEY (address_id) REFERENCES rel.addresses (id)
);