/* jshint indent: 2 */
// tslint:disable
import * as sequelize from 'sequelize';
import {DataTypes} from 'sequelize';
import {trsSaledetailInstance, trsSaledetailAttribute} from './db';

module.exports = function(sequelize: sequelize.Sequelize, DataTypes: DataTypes) {
  return sequelize.define<trsSaledetailInstance, trsSaledetailAttribute>('trsSaledetail', {
    invid: {
      type: DataTypes.BIGINT,
      allowNull: true
    },
    itemid: {
      type: DataTypes.BIGINT,
      allowNull: true
    },
    taxper: {
      type: DataTypes.FLOAT,
      allowNull: true,
      defaultValue: '((0))'
    },
    rate: {
      type: "MONEY",
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
      allowNull: true,
      defaultValue: '((0))'
    },
    grossAmount: {
      type: "MONEY",
      allowNull: true,
      defaultValue: '((0))'
    },
    discount: {
      type: "MONEY",
      allowNull: true,
      defaultValue: '((0))'
    },
    taxable: {
      type: "MONEY",
      allowNull: true
    },
    taxAmount: {
      type: "MONEY",
      allowNull: true
    },
    netAmount: {
      type: "MONEY",
      allowNull: true
    },
    Batch: {
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
    }
  }, {
    tableName: 'trsSaledetail'
  });
};
