import { Component, Inject, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { AuthorizeService } from '../../api-authorization/authorize.service';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent implements OnInit{
  isExpanded = false;
  public isAdmin: Observable<boolean>;
  public isAuthenticated: Observable<boolean>;
  constructor(private authorizeService: AuthorizeService, private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
    
  }

  ngOnInit() {
    this.isAdmin = this.http.get<boolean>(this.baseUrl + `api/user`);
    this.isAuthenticated = this.authorizeService.isAuthenticated();
  }

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }
}
