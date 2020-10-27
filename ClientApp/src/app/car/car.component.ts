import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-car-component',
  templateUrl: './car.component.html'
})
export class CarComponent {
  public car: Car;
  id: string;
  carAvailable: boolean;
  today: Date;
  dates = { from: undefined, to: undefined};

  constructor(private fb: FormBuilder, private route: ActivatedRoute, private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
   
    this.carAvailable = false;
    this.today = new Date();

  }
  ngOnInit() {
    this.id = this.route.snapshot.paramMap.get('id');
    this.http.get<Car>(this.baseUrl + `api/car/${this.id}`).subscribe(result => {
      this.car = result;
    }, error => console.error(error));
  }

  createImagePath(path: string) {
    path.replace("\\", "/");
    return `https://localhost:44311/${path}`;
  }

  reserveCar() {
    // toDo
  }

  checkIfAvailable() {
    console.log(this.dates.from);
    if (this.dates.from != undefined && this.dates.to != undefined) {
      this.carAvailable = true;
      //console.log(this.form.get('fromDate').value);
      //this.http.post<Car>(this.baseUrl + `api/car`, this.form.value).subscribe((result) => {
      //  console.log(result)
      //}, error => { console.log(error); });
    } else {
    //  this.openSnackBar("Please complete all required fields", "ok");
    }
  }
  clear() {
    this.dates = { from: undefined, to: undefined };
    this.carAvailable = false;
  }
}

interface Car {
  carID: string;
  image: string;
  brand: string;
  productionYear: string;
  color: string;
  doors: string;
  seats: string;
  manualGearbox: boolean;
  airConditioning: boolean;
}
