import { Component, OnInit } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { User } from '../_models/User';
import { Observable } from 'rxjs';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css'],
})
export class NavComponent implements OnInit {
  model: any = {};
  //loggedIn: boolean;
  currentUser$: Observable<User>;
  constructor(
    public accountService: AccountService,
    private router: Router,
    private toastr: ToastrService
  ) {}
  ngOnInit(): void {
    this.currentUser$ = this.accountService.currentUser$;
    // this.getCurrentUser();
  }
  logout() {
    this.accountService.logout();
    this.router.navigateByUrl('/');
  }
  login() {
    console.log(this.model);
    this.accountService.login(this.model).subscribe((response) => {
      this.router.navigateByUrl('./members');
      console.log(response);
    });
  }

  // getCurrentUser() {
  //   this.accountService.currentUser$.subscribe(
  //     (user) => {
  //       this.loggedIn = !!user;
  //     },
  //     (erorr) => {
  //       console.log(erorr);
  //     }
  //   );
  // }
}
