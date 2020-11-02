import { Component, Inject } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {
  isExpanded = false;
  public isAdmin: Observable<boolean>;
  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
    this.isAdmin = this.http.get<boolean>(this.baseUrl + `api/user`);
  }

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }
}
