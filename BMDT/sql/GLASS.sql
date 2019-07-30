/*
 Navicat Premium Data Transfer

 Source Server         : BMDT
 Source Server Type    : SQLite
 Source Server Version : 3017000
 Source Schema         : main

 Target Server Type    : SQLite
 Target Server Version : 3017000
 File Encoding         : 65001

 Date: 30/07/2019 15:02:17
*/

PRAGMA foreign_keys = false;

-- ----------------------------
-- Table structure for GLASS
-- ----------------------------
DROP TABLE IF EXISTS "GLASS";
CREATE TABLE "GLASS" (
  "PNLID" VARCHAR(20) NOT NULL,
  "STARTTIME" DATETIME,
  "ENDTIME" DATETIME,
  "STAGEID" INTEGER,
  "UNITID" VARCHAR(20),
  "COSTTIME" INTEGER,
  "BLUID" VARCHAR(50),
  "PNLJUDGE" VARCHAR(3),
  "STATE" INTEGER,
  PRIMARY KEY ("PNLID")
);

PRAGMA foreign_keys = true;
