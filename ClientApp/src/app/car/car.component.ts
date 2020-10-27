import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-car-component',
  templateUrl: './car.component.html'
})
export class CarComponent {
  public car: Car;
  id: string;

  constructor(private route: ActivatedRoute, private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
   
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
