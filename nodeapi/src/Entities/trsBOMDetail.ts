/* jshint indent: 2 */
// tslint:disable
import * as sequelize from 'sequelize';
import {DataTypes} from 'sequelize';
import {trsBOMDetailInstance, trsBOMDetailAttribute} from './db';

module.exports = function(sequelize: sequelize.Sequelize, DataTypes: DataTypes) {
  return sequelize.define<trsBOMDetailInstance, trsBOMDetailAttribute>('trsBOMDetail', {
    bomId: {
      type: DataTypes.INTEGER,
      allowNull: false
    },
    prodID: {
      type: DataTypes.BIGINT,
      allowNull: false
    },
    qty: {
      type: DataTypes.FLOAT,
      allowNull: true
    },
    rOrder: {
      type: DataTypes.INTEGER,
      allowNull: true
    },
    bomMode: {
      type: DataTypes.INTEGER,
      allowNull: true
    },
    blnAltUnit: {
      type: DataTypes.BOOLEAN,
      allowNull: true,
      defaultValue: '0'
    },
    SSMA_TimeStamp: {
      type: DataTypes.DATE,
      allowNull: false
    }
  }, {
    tableName: 'trsBOMDetail'
  });
};
