/* jshint indent: 2 */
// tslint:disable
import * as sequelize from 'sequelize';
import {DataTypes} from 'sequelize';
import {mtrledgerInstance, mtrledgerAttribute} from './db';

module.exports = function(sequelize: sequelize.Sequelize, DataTypes: DataTypes) {
  return sequelize.define<mtrledgerInstance, mtrledgerAttribute>('mtrledger', {
    lid: {
      type: DataTypes.BIGINT,
      allowNull: false,
      primaryKey: true
    },
    ledger: {
      type: DataTypes.STRING,
      allowNull: false
    },
    LedgerName: {
      type: DataTypes.STRING,
      allowNull: true
    },
    accid: {
      type: DataTypes.BIGINT,
      allowNull: true
    },
    contactno: {
      type: DataTypes.STRING,
      allowNull: true
    },
    phoneNo: {
      type: DataTypes.STRING,
      allowNull: true
    },
    address: {
      type: DataTypes.STRING,
      allowNull: true
    },
    tinno: {
      type: DataTypes.STRING,
      allowNull: true
    },
    areaid: {
      type: DataTypes.BIGINT,
      allowNull: true,
      defaultValue: '((0))'
    },
    deActive: {
      type: DataTypes.BOOLEAN,
      allowNull: true,
      defaultValue: '0'
    },
    plID: {
      type: DataTypes.INTEGER,
      allowNull: true
    },
    email: {
      type: DataTypes.STRING,
      allowNull: true
    },
    loyaltyID: {
      type: DataTypes.INTEGER,
      allowNull: true
    },
    regionalName: {
      type: DataTypes.STRING,
      allowNull: true
    },
    remarks: {
      type: DataTypes.STRING,
      allowNull: true
    },
    DiscPer: {
      type: DataTypes.FLOAT,
      allowNull: true,
      defaultValue: '((0))'
    },
    SSMA_TimeStamp: {
      type: DataTypes.DATE,
      allowNull: false
    },
    creditlimit: {
      type: DataTypes.FLOAT,
      allowNull: true
    }
  }, {
    tableName: 'mtrledger'
  });
};
