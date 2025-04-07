import { Component, OnInit } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { CuisineService } from '../../services/cuisine.service';
import { CommonModule } from '@angular/common';
import { Cuisine } from '../../models/cuisine';


@Component({
  selector: 'app-cuisines-page',
  imports: [RouterModule, CommonModule],
  templateUrl: './cuisines-page.component.html',
  styleUrl: './cuisines-page.component.css'
})
export class CuisinesPageComponent implements OnInit {
  cuisines:  Cuisine[] = [];;
  constructor(private cuisineService: CuisineService, private router: Router){}

  ngOnInit(): void {
    this.cuisineService.getCuisines().subscribe((cuisines: Cuisine[]) => {
      this.cuisines = cuisines;
    });
  }

  transformImage(url: string): string {
    return url.replace('/upload/', '/upload/h_300,w_300/');
  }

  navigateToCuisine(cuisine: any) {
    this.router.navigate(['/sign-in' ]); // navigate to meals page with cuisine.id
  }

}
