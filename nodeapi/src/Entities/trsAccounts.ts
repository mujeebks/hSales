/* jshint indent: 2 */
// tslint:disable
import * as sequelize from 'sequelize';
import {DataTypes} from 'sequelize';
import {trsAccountsInstance, trsAccountsAttribute} from './db';

module.exports = function(sequelize: sequelize.Sequelize, DataTypes: DataTypes) {
  return sequelize.define<trsAccountsInstance, trsAccountsAttribute>('trsAccounts', {
    Invid: {
      type: DataTypes.BIGINT,
      allowNull: true
    },
    InvType: {
      type: DataTypes.STRING,
      allowNull: true
    },
    InvDate: {
      type: DataTypes.DATE,
      allowNull: true
    },
    Drlid: {
      type: DataTypes.BIGINT,
      allowNull: true
    },
    Crlid: {
      type: DataTypes.BIGINT,
      allowNull: true
    },
    AmountD: {
      type: "MONEY",
      allowNull: true
    },
    AmountC: {
      type: "MONEY",
      allowNull: true
    },
    invNo: {
      type: DataTypes.STRING,
      allowNull: true
    },
    remarks: {
      type: DataTypes.STRING,
      allowNull: true
    },
    IsHold: {
      type: DataTypes.BOOLEAN,
      allowNull: true
    }
  }, {
    tableName: 'trsAccounts'
  });
};
