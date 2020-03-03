import { Subject } from 'rxjs/Rx';
import { UserFormDialogComponent } from './user-form-dialog/user-form-dialog.component';
import { Component, OnInit, ViewChild } from '@angular/core';
import { MatSidenav, MatDialogConfig, MatDialog } from '@angular/material';
import { UserDetails } from 'app/administration/components/users/models/user.model';
import { UsersService } from 'app/administration/users.service';

import { SnackBarHelper } from '../../../core/snack-bar-helper.service';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.scss']
})
export class UsersComponent implements OnInit {
  reset = new Subject();
  showForm = false;
  selectedUser: UserDetails;

  constructor(
    private usersService: UsersService,
    private snackBarHelper: SnackBarHelper,
    public dialog: MatDialog) { }

  ngOnInit() {
  }

  openDialog() {
    const config = new MatDialogConfig();
    config.disableClose = true;
    config.width = '80vw';
    config.height = '90vh';
    config.data = {
      model: this.selectedUser,
    };

    const dialogRef = this.dialog.open(UserFormDialogComponent, config);

    dialogRef.afterClosed()
      .subscribe((result: UserDetails) => {
        if (result) {
          this.saveUser(result);
        }
        this.selectedUser = null;
      });
  }

  saveUser(value: UserDetails) {
    (!value.id
      ? this.usersService.addUser(value)
      : this.usersService.updateUser(value))
      .subscribe(
        () => this.snackBarHelper.success('User saved'),
        err => { },
        () => this.reset.next()
      );
  }

  selectUser(value: any) {
    if (isNaN(value) === false && value > 0) {
      this.usersService.getById(value)
        .subscribe((data: UserDetails) => {
          this.selectedUser = data;
          this.openDialog();
        },
          error => this.snackBarHelper.error(error))
    }
  }
}
