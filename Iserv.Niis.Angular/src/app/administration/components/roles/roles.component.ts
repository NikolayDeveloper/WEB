import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig, MatSidenav } from '@angular/material';

import { RoleFormDialogComponent } from 'app/administration/components/roles/role-form-dialog/role-form-dialog.component';
import { SnackBarHelper } from '../../../core/snack-bar-helper.service';
import { RolesService } from '../../roles.service';
import { Permission, RoleDetails } from './models/role.models';

@Component({
  selector: 'app-roles',
  templateUrl: './roles.component.html',
  styleUrls: ['./roles.component.scss']
})
export class RolesComponent implements OnInit {
  showForm = false;
  private selectedRole: RoleDetails;
  private permissions: Permission[];
  private stagesTree: any;

  constructor(
    private rolesService: RolesService,
    private snackBarHelper: SnackBarHelper,
    public dialog: MatDialog) { }

  ngOnInit() {
    this.initialize();
  }

  saveRole(value: RoleDetails) {
    if (!value.id) {
      value.id = 0;
      this.rolesService.addRole(value)
        .subscribe(() => this.snackBarHelper.success('Role saved'));
    } else {
      this.rolesService.updateRole(value)
        .subscribe(() => this.snackBarHelper.success('Role saved'))
    }
  }

  selectRole(value: any) {
    this.rolesService.getById(value)
      .subscribe((data: RoleDetails) => {
        this.selectedRole = data;
        this.openDialog();
      },
      error => this.snackBarHelper.error(error));
  }

  cancelEdit(value: boolean) {
    this.selectedRole = new RoleDetails();
  }

  openDialog() {
    const config = new MatDialogConfig();
    config.disableClose = true;
    config.width = '80vw';
    config.height = '90vh';
    config.data = {
      model: this.selectedRole,
      perms: this.permissions,
      stagesTree: this.stagesTree,
    }

    const dialogRef = this.dialog.open(RoleFormDialogComponent, config);

    dialogRef.afterClosed()
      .subscribe((result: RoleDetails) => {
        if (result) {
          this.saveRole(result);
        }
        this.selectedRole = null;
      });
  }

  private initialize() {
    this.rolesService
      .getPermissions()
      .subscribe((data: Permission[]) => this.permissions = data)
    this.rolesService
      .getStagesTree()
      .subscribe(data => { this.stagesTree = data });
  }
}
