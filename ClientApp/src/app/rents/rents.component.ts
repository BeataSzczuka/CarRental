import { Component, Inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { MatSnackBar } from '@angular/material';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-rents-component',
  templateUrl: './rents.component.html',
  styleUrls: ['./rents.component.scss']
})
export class RentsComponent {
  public rents: Rent[] = [];

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string, private _snackBar: MatSnackBar) {
    this.loadRents();
  }

  delete(id) {
    this.http.delete<Rent>(this.baseUrl + `api/rent/${id}`).subscribe(result => {
      this.rents = this.rents.filter((rent) => rent.rentID != id);
      this.openSnackBar("Rent has been successfully deleted", "ok");
    }, error => this.openSnackBar("Something went wrong", "ok"));
  }

 
  loadRents() {
    this.http.get<Rent[]>(this.baseUrl + 'api/rent').subscribe(result => {
      this.rents = result;
    },
      error => console.error(error)
    );
  }

  openSnackBar(message: string, action: string) {
    this._snackBar.open(message, action, {
      duration: 2000,
    });
  }
}

interface Rent {
  rentID: string;
  username: string;
  dateFrom: string;
  dateTo: string;
  carID: string;
}
