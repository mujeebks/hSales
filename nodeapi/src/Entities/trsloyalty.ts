/* jshint indent: 2 */
// tslint:disable
import * as sequelize from 'sequelize';
import {DataTypes} from 'sequelize';
import {trsloyaltyInstance, trsloyaltyAttribute} from './db';

module.exports = function(sequelize: sequelize.Sequelize, DataTypes: DataTypes) {
  return sequelize.define<trsloyaltyInstance, trsloyaltyAttribute>('trsloyalty', {
    SaleID: {
      type: DataTypes.BIGINT,
      allowNull: false,
      primaryKey: true
    },
    loyaltyID: {
      type: DataTypes.INTEGER,
      allowNull: false
    },
    LID: {
      type: DataTypes.INTEGER,
      allowNull: false
    },
    saleAmount: {
      type: DataTypes.FLOAT,
      allowNull: true
    },
    PointIn: {
      type: DataTypes.FLOAT,
      allowNull: true
    },
    PointOut: {
      type: DataTypes.FLOAT,
      allowNull: true
    },
    SSMA_TimeStamp: {
      type: DataTypes.DATE,
      allowNull: false
    }
  }, {
    tableName: 'trsloyalty'
  });
};
