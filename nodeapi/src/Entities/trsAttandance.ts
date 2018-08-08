/* jshint indent: 2 */
// tslint:disable
import * as sequelize from 'sequelize';
import {DataTypes} from 'sequelize';
import {trsAttandanceInstance, trsAttandanceAttribute} from './db';

module.exports = function(sequelize: sequelize.Sequelize, DataTypes: DataTypes) {
  return sequelize.define<trsAttandanceInstance, trsAttandanceAttribute>('trsAttandance', {
    AttID: {
      type: DataTypes.FLOAT,
      allowNull: true,
      defaultValue: '((0))'
    },
    employeeID: {
      type: DataTypes.INTEGER,
      allowNull: true
    },
    InvDate: {
      type: DataTypes.DATE,
      allowNull: true
    },
    status: {
      type: DataTypes.STRING,
      allowNull: true
    },
    Quantity: {
      type: DataTypes.INTEGER,
      allowNull: true,
      defaultValue: '((0))'
    },
    Amount: {
      type: DataTypes.INTEGER,
      allowNull: true
    },
    TotAmount: {
      type: DataTypes.INTEGER,
      allowNull: true
    },
    Advance: {
      type: DataTypes.INTEGER,
      allowNull: true
    },
    Incentive: {
      type: DataTypes.INTEGER,
      allowNull: true
    },
    SSMA_TimeStamp: {
      type: DataTypes.DATE,
      allowNull: false
    }
  }, {
    tableName: 'trsAttandance'
  });
};
