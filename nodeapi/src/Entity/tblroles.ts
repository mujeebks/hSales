/* jshint indent: 2 */
// tslint:disable
import * as sequelize from 'sequelize';
import {DataTypes} from 'sequelize';
import {tblrolesInstance, tblrolesAttribute} from './db';

module.exports = function(sequelize: sequelize.Sequelize, DataTypes: DataTypes) {
  return sequelize.define<tblrolesInstance, tblrolesAttribute>('tblroles', {
    RoleId: {
      type: DataTypes.INTEGER,
      allowNull: false,
      primaryKey: true,
      autoIncrement: true
    },
    RoleDescription: {
      type: DataTypes.STRING,
      allowNull: true
    }
  }, {
    tableName: 'tblroles',
    timestamps: false
  });
};
