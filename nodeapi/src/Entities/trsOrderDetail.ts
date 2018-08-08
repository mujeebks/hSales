/* jshint indent: 2 */
// tslint:disable
import * as sequelize from 'sequelize';
import {DataTypes} from 'sequelize';
import {trsOrderDetailInstance, trsOrderDetailAttribute} from './db';

module.exports = function(sequelize: sequelize.Sequelize, DataTypes: DataTypes) {
  return sequelize.define<trsOrderDetailInstance, trsOrderDetailAttribute>('trsOrderDetail', {
    invID: {
      type: DataTypes.BIGINT,
      allowNull: false
    },
    itemid: {
      type: DataTypes.BIGINT,
      allowNull: true
    },
    taxper: {
      type: DataTypes.FLOAT,
      allowNull: true
    },
    rate: {
      type: DataTypes.FLOAT,
      allowNull: true
    },
    mrp: {
      type: "MONEY",
      allowNull: true,
      defaultValue: '((0))'
    },
    quantity: {
      type: DataTypes.FLOAT,
      allowNull: true
    },
    freeQty: {
      type: DataTypes.FLOAT,
      allowNull: true
    },
    grossAmount: {
      type: DataTypes.FLOAT,
      allowNull: true
    },
    Discount: {
      type: DataTypes.FLOAT,
      allowNull: true
    },
    taxable: {
      type: DataTypes.FLOAT,
      allowNull: true
    },
    taxAmount: {
      type: DataTypes.FLOAT,
      allowNull: true
    },
    netAmount: {
      type: DataTypes.FLOAT,
      allowNull: true
    },
    batch: {
      type: DataTypes.STRING,
      allowNull: true
    },
    cost: {
      type: DataTypes.FLOAT,
      allowNull: true,
      defaultValue: '((0))'
    },
    saleValue: {
      type: DataTypes.FLOAT,
      allowNull: true,
      defaultValue: '((0))'
    },
    profit: {
      type: DataTypes.FLOAT,
      allowNull: true,
      defaultValue: '((0))'
    },
    rOrder: {
      type: DataTypes.INTEGER,
      allowNull: true
    },
    extraColumn: {
      type: DataTypes.STRING,
      allowNull: true
    },
    SSMA_TimeStamp: {
      type: DataTypes.DATE,
      allowNull: false
    },
    unitfactor: {
      type: DataTypes.FLOAT,
      allowNull: true
    },
    blnAltUnit: {
      type: DataTypes.BOOLEAN,
      allowNull: true
    },
    unit: {
      type: DataTypes.STRING,
      allowNull: true
    },
    expDate: {
      type: DataTypes.STRING,
      allowNull: true
    },
    sRate: {
      type: DataTypes.FLOAT,
      allowNull: true
    }
  }, {
    tableName: 'trsOrderDetail'
  });
};
