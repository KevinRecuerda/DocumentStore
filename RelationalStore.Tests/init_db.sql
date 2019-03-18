
CREATE SCHEMA IF NOT EXISTS rel;

DROP TABLE IF EXISTS rel.users     CASCADE;
DROP TABLE IF EXISTS rel.addresses CASCADE;

DROP TABLE IF EXISTS rel.issues     CASCADE;
DROP TABLE IF EXISTS rel.issue_tags CASCADE;


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


CREATE TABLE rel.issues (
    id          BIGSERIAL PRIMARY KEY,
    name        TEXT,
    assignee_id TEXT,
    reporter_id TEXT
);


CREATE TABLE rel.issue_tags (
    issue_id    BIGINT  NOT NULL,
    name        TEXT    NOT NULL,
    CONSTRAINT fk__issue_tags__issues FOREIGN KEY (issue_id) REFERENCES rel.issues (id)
);