import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { NgxPermissionsService } from 'ngx-permissions';
import { TokenStorageService } from './shared/authentication/token-storage.service';
import { SystemService } from './shared/services/system.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  year = (new Date()).getFullYear();
  version: string;
  date: string;

  constructor(
    private tokenStorageService: TokenStorageService,
    private permissionsService: NgxPermissionsService,
    private systemService: SystemService,
    private router: Router
  ) {}

  ngOnInit(): void {
    const profilePrms = this.tokenStorageService.getProfilePermissions();
    if (!profilePrms) {
      this.router.navigateByUrl('/login');
    } else {
      this.permissionsService.addPermission(profilePrms);
    }

    this.systemService.getVersion().subscribe((data: any) => {
      this.version = `${data.major}.${data.minor}.${data.build}.${data.revision}`;
      this.date = data.date.toLocaleDateString();
    });
  }
}
