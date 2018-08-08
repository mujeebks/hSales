/* jshint indent: 2 */
// tslint:disable
import * as sequelize from 'sequelize';
import {DataTypes} from 'sequelize';
import {trsStockQtyInstance, trsStockQtyAttribute} from './db';

module.exports = function(sequelize: sequelize.Sequelize, DataTypes: DataTypes) {
  return sequelize.define<trsStockQtyInstance, trsStockQtyAttribute>('trsStockQty', {
    ItemID: {
      type: DataTypes.BIGINT,
      allowNull: true
    },
    Batch: {
      type: DataTypes.STRING,
      allowNull: true
    },
    Prate: {
      type: DataTypes.FLOAT,
      allowNull: true
    },
    SRate: {
      type: DataTypes.FLOAT,
      allowNull: true
    },
    Mrp: {
      type: DataTypes.FLOAT,
      allowNull: true
    },
    cost: {
      type: DataTypes.FLOAT,
      allowNull: true,
      defaultValue: '((0))'
    },
    intBatch: {
      type: DataTypes.BIGINT,
      allowNull: true
    },
    SSMA_TimeStamp: {
      type: DataTypes.DATE,
      allowNull: false
    },
    deActive: {
      type: DataTypes.BOOLEAN,
      allowNull: true
    },
    expDate: {
      type: DataTypes.STRING,
      allowNull: true
    }
  }, {
    tableName: 'trsStockQty'
  });
};
