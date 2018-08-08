/* jshint indent: 2 */
// tslint:disable
import * as sequelize from 'sequelize';
import {DataTypes} from 'sequelize';
import {trsStockInstance, trsStockAttribute} from './db';

module.exports = function(sequelize: sequelize.Sequelize, DataTypes: DataTypes) {
  return sequelize.define<trsStockInstance, trsStockAttribute>('trsStock', {
    Invid: {
      type: DataTypes.BIGINT,
      allowNull: true
    },
    InvType: {
      type: DataTypes.STRING,
      allowNull: true
    },
    Invdate: {
      type: DataTypes.DATE,
      allowNull: true
    },
    itemid: {
      type: DataTypes.BIGINT,
      allowNull: true
    },
    qtyin: {
      type: DataTypes.FLOAT,
      allowNull: true
    },
    qtyout: {
      type: DataTypes.FLOAT,
      allowNull: true
    },
    Batch: {
      type: DataTypes.STRING,
      allowNull: true
    },
    SSMA_TimeStamp: {
      type: DataTypes.DATE,
      allowNull: false
    },
    mrp: {
      type: DataTypes.FLOAT,
      allowNull: true
    },
    sRate: {
      type: DataTypes.FLOAT,
      allowNull: true
    },
    pRate: {
      type: DataTypes.FLOAT,
      allowNull: true
    },
    lid: {
      type: DataTypes.INTEGER,
      allowNull: true
    },
    invNo: {
      type: DataTypes.STRING,
      allowNull: true
    }
  }, {
    tableName: 'trsStock'
  });
};
